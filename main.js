// global variables for other classes
var numberOfRings = 0;
var ringImageHalfWidth = 0;
var ringImageHalfHeight = 0;

var letterImage = null;
var bodies = null;
var currentPage;

// local namespace
$(function() 
{
    var jsonData;
    var context = $('#letterCanvas')[0].getContext('2d');
    var canvasSize = {w: context.canvas.width, h: context.canvas.height};
    var worldSize = {w: context.canvas.width / SCALE, h: context.canvas.height / SCALE};

    var SCALE = 30;
    var ringIndizes = [];
    var physicWorld = null;
    var backImage = null;

    // see http://www.jquery4u.com/snippets/url-parameters-jquery/
    $.urlParam = function(name)
    {
        var results = new RegExp('[\\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
        // default: page 1
        if(results == null)
            return 1;
        return results[1];
    }

    $(document).ready(function()
    {
        bodies = new Array();
        ringIndizes = new Array();
        currentPage = $.urlParam('page');

        letterImage = new Image();
        letterImage.src = currentPage + "/letters.png";
        backImage = new Image();
        backImage.src = currentPage + "/back.png";
        ringImage = new Image();
        ringImage.src = currentPage + "/ring.png";

        ringImage.onload = function() 
        {
            ringImageHalfWidth = ringImage.width / 2;
            ringImageHalfHeight = ringImage.height / 2;
        }

        $.ajax(
        {
            url: currentPage + "/bodies.json", 
            dataType: 'json'
        }).done(function(data) 
        {
            jsonData = data;
            $.initBodies();
        });
    });

    $('#reload').click(function() 
    {
        $.initBodies();
    });

    // interaction events
    $(function() 
    {
        var leftButtonDown = false;
        $(document).mousemove(function(e)
        {
            if(leftButtonDown)
                physicWorld.updateJointAtMouse({x: e.pageX, y: e.pageY});
        });

        $(document).bind('touchmove', function(e)
        {
            // see http://www.devinrolsen.com/basic-jquery-touchmove-event-setup/
            e.preventDefault();
            var touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
            var elm = $(this).offset();
            var touchX, touchY;
            if(elm === undefined) {
                touchX = touch.pageX;
                touchY = touch.pageY;
            }
            else {
                touchX = touch.pageX - elm.left;
                touchY = touch.pageY - elm.top;
            }
            if(     touchX < $(this).width() && touchX > 0
                &&  touchY < $(this).height() && touchY > 0)
                physicWorld.updateJointAtMouse({x: touchX, y: touchY});
        });

        $(document).mousedown(function(e) 
        {
            if(e.which === 1)
                leftButtonDown = true;
        });

        $(document).bind('touchstart', function()
        {
            leftButtonDown = true;
        });

        $(document).mouseup(function(e) 
        {
            if(e.which === 1)
                removeMouseJoint();
        });

        $(document).bind('touchend', function()
        {
            removeMouseJoint();
        });

        function removeMouseJoint()
        {
            leftButtonDown = false;
            physicWorld.removeJointAtMouse();
        }
    });
       
    $.initBodies = function()
    { 
        numberOfRings = 0;

        for(var i in jsonData) 
            bodies[i] = Entity.build(i, jsonData[i]);
        
        physicWorld = new PhysicWorld(60, false, canvasSize.w, canvasSize.h, SCALE);
        physicWorld.setBodies(bodies);

        ($.loop = function(animStart) 
        {
            $.update(animStart);
            $.draw();
            requestAnimFrame($.loop);
        })();
    }

    $.update = function(animStart)
    {
        physicWorld.update();
        physicWorld.updateBodies(bodies, worldSize);
    }

    $.draw = function()
    {
        context.drawImage(backImage, 0, 0);
        $.drawBlackSquares();
        for (var i in bodies) 
            bodies[i].draw(context);
    }

    // "fakes" transparency for rings
    $.drawBlackSquares = function()
    {
        context.fillStyle = '#000000';

        if(numberOfRings == 1)
        {
            // (make all rectangles 2 pixel larger in size because of float/int inaccuracy)
            // left (aligned to ring)
            context.fillRect(0, bodies[0].y - ringImageHalfHeight, bodies[0].x - ringImageHalfWidth + 2, ringImage.height);
            // right
            context.fillRect(bodies[0].x + ringImageHalfWidth - 2, bodies[0].y - ringImageHalfHeight, canvasSize.w - (bodies[0].x + ringImageHalfWidth) + 2, ringImage.height);
            // top
            context.fillRect(0, 0, canvasSize.w, bodies[0].y - ringImageHalfHeight + 2);
            // bottom
            context.fillRect(0, bodies[0].y + ringImageHalfHeight - 2, canvasSize.w, canvasSize.h - (bodies[0].y + ringImageHalfHeight) + 2);
        }
    }

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
});