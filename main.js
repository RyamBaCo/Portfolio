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
var letters = null;
var physicWorld = null;
var letterImage = null;

$(document).ready(function()
{
    letters = new Array();
    letterImage = new Image();
    letterImage.src = "letters.png";

    $.ajax(
    {
        url: "letters.json", 
        dataType: 'json'
    }).done(function(data) 
    {
        initLetters(data);
    });
});
                    
function initLetters(jsonData) 
{ 
    for(var i in jsonData) 
        letters[i] = Entity.build(i, jsonData[i]);
    
    physicWorld = new PhysicWorld(60, false, canvasWidth, canvasHeight, SCALE);
    physicWorld.setLetters(letters);

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
    physicWorld.updateLetters(letters);
}

function draw() 
{
    context.fillStyle = '#000000';
    context.fillRect(0, 0, canvasWidth, canvasHeight);
    for (var i in letters) 
        letters[i].draw(context);
}