window.requestAnimFrame = (function()
{
  return  window.requestAnimationFrame       || 
          window.webkitRequestAnimationFrame || 
          window.mozRequestAnimationFrame    || 
          window.oRequestAnimationFrame      || 
          window.msRequestAnimationFrame     || 
          function(callback, element)
          {
              window.setTimeout(callback, 1000 / 60);
          };
})();

var context = $('#letterCanvas')[0].getContext('2d');
var canvasWidth = context.canvas.width;
var canvasHeight = context.canvas.height;

var SCALE = 30;
var bodies = null;
var physicWorld = null;
var letterImage = null;

$(document).ready(function()
{
    bodies = new Array();
    letterImage = new Image();
    letterImage.src = "letters.png";

    $.ajax(
    {
        url: "letters.json", 
        dataType: 'json'
    }).done(function(data) 
    {
        initBodies(data);
    });
});
                    
function initBodies(jsonData) 
{ 
    for(var i in jsonData) 
    {
        bodies[i] = Entity.build(i, jsonData[i]);
    }
    
    physicWorld = new PhysicWorld(60, false, canvasWidth, canvasHeight, SCALE);
    physicWorld.setBodies(bodies);

    (function loop(animStart) 
    {
        update(animStart);
        draw();
        requestAnimFrame(loop);
    })();
}

function update(animStart) 
{
    physicWorld.update();
    physicWorld.updateBodies(bodies);
}

function draw() 
{
    context.fillStyle = '#000000';
    context.fillRect(0, 0, canvasWidth, canvasHeight);
    for (var i in bodies) 
    {
        var entity = bodies[i];
        entity.draw(context);
    }
}