var   b2Vec2 = Box2D.Common.Math.b2Vec2,
      b2BodyDef = Box2D.Dynamics.b2BodyDef,
      b2Body = Box2D.Dynamics.b2Body,
      b2FixtureDef = Box2D.Dynamics.b2FixtureDef,
      b2Fixture = Box2D.Dynamics.b2Fixture,
      b2World = Box2D.Dynamics.b2World,
      b2MassData = Box2D.Collision.Shapes.b2MassData,
      b2PolygonShape = Box2D.Collision.Shapes.b2PolygonShape,
      b2CircleShape = Box2D.Collision.Shapes.b2CircleShape,
      b2DebugDraw = Box2D.Dynamics.b2DebugDraw;

function PhysicWorld(intervalRate, adaptive, width, height, scale) 
{
    this.intervalRate = parseInt(intervalRate);
    this.adaptive = adaptive;
    this.width = width;
    this.height = height;
    this.scale = scale;

    this.world = new b2World(new b2Vec2(0, 10), true);

    this.fixDef = new b2FixtureDef;
    this.fixDef.density = 1.0;
    this.fixDef.friction = 0.5;
    this.fixDef.restitution = 0.2;

    this.bodyDef = new b2BodyDef;

    //create ground
    this.bodyDef.type = b2Body.b2_staticBody;

    // positions the center of the object (not upper left!)
    this.bodyDef.position.x = this.width / 2 / this.scale;
    this.bodyDef.position.y = this.height / this.scale;

    this.fixDef.shape = new b2PolygonShape;

    // half width, half height. eg actual height here is 1 unit
    this.fixDef.shape.SetAsBox((this.width- (this.width * 0.1) / this.scale) / 2, (10 / this.scale) / 2);
    this.world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);
}

PhysicWorld.prototype.update = function() 
{
    var start = Date.now();
    var stepRate = (this.adaptive) ? (now - this.lastTimestamp) / 1000 : (1 / this.intervalRate);
    this.world.Step(stepRate, 5, 5);
    this.world.ClearForces();
    return (Date.now() - start);
}

PhysicWorld.prototype.updateLetters = function(letters) 
{
    for (var b = this.world.GetBodyList(); b; b = b.m_next) 
    {
        if (b.IsActive() && typeof b.GetUserData() !== 'undefined' && b.GetUserData() != null) 
            letters[b.GetUserData()].update(
              { 
                  x: b.GetPosition().x * this.scale, 
                  y: b.GetPosition().y * this.scale, 
                  a: b.GetAngle(), 
                  c: {x: b.GetWorldCenter().x * this.scale, y: b.GetWorldCenter().y * this.scale
              }});
    }
}

PhysicWorld.prototype.setLetters = function(letters) 
{
    this.bodyDef.type = b2Body.b2_dynamicBody;
    for(var i in letters) 
    {
        var entity = letters[i];
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
        this.world.CreateBody(this.bodyDef).CreateFixture(this.fixDef);
    }
}