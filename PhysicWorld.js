var   b2Vec2 = Box2D.Common.Math.b2Vec2,
      b2AABB = Box2D.Collision.b2AABB,
      b2BodyDef = Box2D.Dynamics.b2BodyDef,
      b2Body = Box2D.Dynamics.b2Body,
      b2FixtureDef = Box2D.Dynamics.b2FixtureDef,
      b2Fixture = Box2D.Dynamics.b2Fixture,
      b2RevoluteJointDef = Box2D.Dynamics.Joints.b2RevoluteJointDef,
      b2MouseJointDef =  Box2D.Dynamics.Joints.b2MouseJointDef,
      b2World = Box2D.Dynamics.b2World,
      b2MassData = Box2D.Collision.Shapes.b2MassData,
      b2PolygonShape = Box2D.Collision.Shapes.b2PolygonShape,
      b2CircleShape = Box2D.Collision.Shapes.b2CircleShape,
      b2Listener = Box2D.Dynamics.b2ContactListener;

// global because of usage in contact listener
var world;
var letterJoints = [];

PhysicWorld.prototype.CATEGORY_FRAME = 0x0001;
PhysicWorld.prototype.CATEGORY_RING = 0x0002;
PhysicWorld.prototype.CATEGORY_LETTER = 0x0004;

function PhysicWorld(intervalRate, adaptive, width, height, scale) 
{
    this.intervalRate = parseInt(intervalRate);
    this.adaptive = adaptive;
    this.width = width;
    this.height = height;
    this.scale = scale;

    world = new b2World(new b2Vec2(0, 10), true);

    this.fixDef = new b2FixtureDef;
    this.fixDef.density = 1.0;
    this.fixDef.friction = 0.5;
    this.fixDef.restitution = 0.2;

    this.bodyDef = new b2BodyDef;

    this.fixDef.filter.categoryBits = this.CATEGORY_FRAME; 

    // ground
    this.bodyDef.type = b2Body.b2_staticBody;
    this.bodyDef.position.x = (this.width / 2) / this.scale;
    this.bodyDef.position.y = (this.height + 5) / this.scale;
    this.fixDef.shape = new b2PolygonShape;
    this.fixDef.shape.SetAsBox((this.width / this.scale) / 2, 5 / this.scale);
    world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);

    // top
    this.bodyDef.position.y = -5 / this.scale;
    world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);

    // left
    this.bodyDef.position.x = -5 / this.scale;
    this.bodyDef.position.y = (this.height / 2) / this.scale;
    this.fixDef.shape.SetAsBox(5 / this.scale, (this.height / this.scale) / 2);
    world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);

    // right
    this.bodyDef.position.x = (this.width + 5) / this.scale;
    world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);

    this.listener = new b2Listener;
    this.listener.BeginContact = this.beginContact;
    world.SetContactListener(this.listener);

    this.ringBodies = [];
    this.mouseJoint = null;
}

PhysicWorld.prototype.beginContact = function(contact) 
{
    userData1 = parseInt(contact.GetFixtureA().GetBody().GetUserData());
    userData2 = parseInt(contact.GetFixtureB().GetBody().GetUserData());

    // collision with ring
    if(userData1 < numberOfRings || userData2 < numberOfRings)
    {
        jointId = Math.max(userData1, userData2);

        if(letterJoints[jointId])
        {
            world.DestroyJoint(letterJoints[jointId]);
            delete letterJoints[jointId];
        }
    }
}

PhysicWorld.prototype.update = function() 
{
    var start = Date.now();
    var stepRate = (this.adaptive) ? (now - this.lastTimestamp) / 1000 : (1 / this.intervalRate);
    world.Step(stepRate, 5, 5);
    world.ClearForces();
    return (Date.now() - start);
}

PhysicWorld.prototype.updateBodies = function(bodies, worldSize, jsonData)
{
    var deleteThreshold = 200;

    for (var currentBody = world.GetBodyList(); currentBody; currentBody = currentBody.m_next) 
    {
        if(currentBody.IsActive() && typeof currentBody.GetUserData() !== 'undefined' && currentBody.GetUserData() != null) {
            bodies[currentBody.GetUserData()].update(
                {
                    x: currentBody.GetPosition().x * this.scale,
                    y: currentBody.GetPosition().y * this.scale,
                    a: currentBody.GetAngle(),
                    c: {
                        x: currentBody.GetWorldCenter().x * this.scale, y: currentBody.GetWorldCenter().y * this.scale
                    }
                });

            if (currentBody.GetPosition().x < -deleteThreshold
                || currentBody.GetPosition().x > this.width / this.scale + deleteThreshold
                || currentBody.GetPosition().y > this.height / this.scale + deleteThreshold) {
                test = jsonData[parseInt(currentBody.GetUserData())];
                currentBody.SetPositionAndAngle({
                    x: test.x / this.scale,
                    y: test.y / this.scale
                }, 0);

                letterJointDef = new b2RevoluteJointDef();
                letterJointDef.Initialize(currentBody, world.GetGroundBody(), currentBody.GetWorldCenter());
                letterJointDef.lowerAngle = 0;
                letterJointDef.upperAngle = 0;
                letterJointDef.enableLimit = true;
                letterJoints[currentBody.GetUserData()] = world.CreateJoint(letterJointDef);

                bodies[currentBody.GetUserData()].m_alpha = 0.0;
            }
        }
    }
}

PhysicWorld.prototype.updateJointAtMouse = function(mousePosition) 
{
    console.log('update at ' + mousePosition.x + " " + mousePosition.y);

    for(var i in bodies)
    {
        if(i >= numberOfRings)
            break;

        var entity = bodies[i];
        var translatedPosition = new b2Vec2(mousePosition.x - bodies[i].x, mousePosition.y - bodies[i].y);

        if(translatedPosition.LengthSquared() <= bodies[i].radiusSquared)
        {
            if(letterJoints[i])
            {
                world.DestroyJoint(letterJoints[i]);
                delete letterJoints[i];
            }

            if(!this.mouseJoint)
            {
                var mouseJointDef = new b2MouseJointDef;
                mouseJointDef.bodyA = world.GetGroundBody();
                mouseJointDef.bodyB = this.ringBodies[i];
                mouseJointDef.target.Set(mousePosition.x / this.scale, mousePosition.y / this.scale);
                mouseJointDef.collideConnected = true;
                mouseJointDef.maxForce = 500.0 * this.ringBodies[i].GetMass();
                this.mouseJoint = world.CreateJoint(mouseJointDef);
                this.ringBodies[i].SetAwake(true);
            }

            break;
        }
    }

    if(this.mouseJoint)
        this.mouseJoint.SetTarget(new b2Vec2(mousePosition.x / this.scale, mousePosition.y / this.scale));
}

PhysicWorld.prototype.removeJointAtMouse = function()
{
    if(this.mouseJoint)
    {
        world.DestroyJoint(this.mouseJoint);
        this.mouseJoint = null;
    }
} 

PhysicWorld.prototype.setBodies = function(bodies) 
{
    this.bodyDef.type = b2Body.b2_dynamicBody;

    for(var i in bodies) 
    {
        var entity = bodies[i];

        if (entity.radius)
            this.fixDef.shape = new b2CircleShape(entity.radius / this.scale);
        else if (entity.points) 
        {
            var points = [];
            for (var j = 0; i < entity.points.length; j++) 
            {
                var vec = new b2Vec2();
                vec.Set(entity.points[j].x, entity.points[j].y);
                points[j] = vec;
            }
            this.fixDef.shape = new b2PolygonShape;
            this.fixDef.shape.SetAsArray(points, points.length);
        } 
        else 
        {
            this.fixDef.shape = new b2PolygonShape;
            this.fixDef.shape.SetAsBox(entity.halfWidth / this.scale, entity.halfHeight / this.scale);
        }

        this.bodyDef.position.x = entity.x / this.scale;
        this.bodyDef.position.y = entity.y / this.scale;
        this.bodyDef.userData = entity.id;

        newBody = world.CreateBody(this.bodyDef);

        letterJointDef = new b2RevoluteJointDef();
        letterJointDef.Initialize(newBody, world.GetGroundBody(), newBody.GetWorldCenter());
        letterJointDef.lowerAngle = 0;
        letterJointDef.upperAngle = 0;
        letterJointDef.enableLimit = true;
        letterJoints[entity.id] = world.CreateJoint(letterJointDef);

        // letter
        if(entity.radius == null)
        {
            this.fixDef.filter.categoryBits = this.CATEGORY_LETTER;
            this.fixDef.filter.maskBits = this.CATEGORY_LETTER | this.CATEGORY_RING;
            newBody.CreateFixture(this.fixDef);
        }

        // ring
        else
        {
            this.fixDef.filter.categoryBits = this.CATEGORY_RING;
            this.fixDef.filter.maskBits = this.CATEGORY_LETTER | this.CATEGORY_FRAME;
            newBody.CreateFixture(this.fixDef);
            this.ringBodies[entity.id] = newBody;
        }
    }
}