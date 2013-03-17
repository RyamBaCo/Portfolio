function Entity(id, x, y) 
{
    this.id = id;
    this.x = x;
    this.y = y;
    this.angle = 0;
    this.center = {x: null, y: null};
}

Entity.prototype.update = function(body) 
{
    this.x = body.x;
    this.y = body.y;
    this.center = body.c;
    this.angle = body.a;
}

Entity.build = function(id, def) 
{
    if (def.r) 
        return new RingEntity(id, def.x, def.y, def.r);
    else if (def.points)
        return new PolygonEntity(id, def.x, def.y, def.points);

    return new LetterEntity(id, def.x, def.y, def.w, def.h);
}

function RingEntity(id, x, y, radius) 
{
    Entity.call(this, id, x, y);
    this.radius = radius;
    this.radiusSquared = radius * radius;

    numberOfRings += 1;
}
RingEntity.prototype = new Entity();
RingEntity.prototype.constructor = RingEntity;

RingEntity.prototype.draw = function(context) 
{
    context.save();
    context.translate(this.x, this.y);
    context.drawImage(ringImage, -ringImageHalfWidth, -ringImageHalfHeight);
    context.restore();
}

function LetterEntity(id, x, y, width, height) 
{
    Entity.call(this, id, x, y);
    this.halfWidth = width / 2;
    this.halfHeight = height / 2;
    this.drawRect = [
        (x - this.halfWidth), (y - this.halfHeight), parseInt(width), parseInt(height), 
        -this.halfWidth, -this.halfHeight, parseInt(width), parseInt(height)];
}
LetterEntity.prototype = new Entity();
LetterEntity.prototype.constructor = LetterEntity;

LetterEntity.prototype.draw = function(context) 
{
    context.save();
    context.translate(this.x, this.y);
    context.rotate(this.angle);
    context.drawImage(
      letterImage,
      this.drawRect[0], this.drawRect[1], 
      this.drawRect[2], this.drawRect[3], 
      this.drawRect[4], this.drawRect[5], 
      this.drawRect[6], this.drawRect[7]);
    context.restore();
}

function PolygonEntity(id, x, y, points) 
{
    Entity.call(this, id, x, y);
    this.points = points;
}
PolygonEntity.prototype = new Entity();
PolygonEntity.prototype.constructor = PolygonEntity;

PolygonEntity.prototype.draw = function(context) 
{
    context.save();
    context.translate(this.x * SCALE, this.y * SCALE);
    context.rotate(this.angle);
    context.translate(-(this.x) * SCALE, -(this.y) * SCALE);
    context.fillStyle = 'red';

    context.beginPath();
    context.moveTo((this.x + this.points[0].x) * SCALE, (this.y + this.points[0].y) * SCALE);

    for (var i = 1; i < this.points.length; i++) 
       context.lineTo((this.points[i].x + this.x) * SCALE, (this.points[i].y + this.y) * SCALE);

    context.lineTo((this.x + this.points[0].x) * SCALE, (this.y + this.points[0].y) * SCALE);
    context.closePath();
    context.fill();
    context.stroke();
    context.restore();
}