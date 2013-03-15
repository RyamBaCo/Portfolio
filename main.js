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
var backImage = null;
var letterImage = null;
var ringImage = null;

$(document).ready(function()
{
    bodies = new Array();

    letterImage = new Image();
    letterImage.src = "letters.png";
    backImage = new Image();
    backImage.src = "back.jpg";
    ringImage = new Image();
    ringImage.src = "ring.png";

    $.ajax(
    {
        url: "bodies.json", 
        dataType: 'json'
    }).done(function(data) 
    {
        initLetters(data);
    });
});
                    
function initLetters(jsonData) 
{ 
    for(var i in jsonData) 
        bodies[i] = Entity.build(i, jsonData[i]);
    
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
    context.drawImage(backImage, 0, 0);
    for (var i in bodies) 
        bodies[i].draw(context);
}