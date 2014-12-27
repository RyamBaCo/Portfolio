// global variables for other classes
var numberOfRings = 0;
var ringImageHalfWidth = 0;
var ringImageHalfHeight = 0;

var letterImage = null;
var bodies = null;
var currentPage;

jQuery(function($) {

    var _oldShow = $.fn.show;

    $.fn.show = function(speed, oldCallback) {
        return $(this).each(function() {
            var obj         = $(this),
                newCallback = function() {
                    if ($.isFunction(oldCallback)) {
                        oldCallback.apply(obj);
                    }
                    obj.trigger('afterShow');
                };

            // you can trigger a before show if you want
            obj.trigger('beforeShow');

            // now use the old function to show the element passing the new callback
            _oldShow.apply(obj, [speed, newCallback]);
        });
    }
});

// local namespace
$(function() 
{
    var jsonData;
    var context;
    var canvasSize;
    var worldSize;

    var SCALE = 30;
    var ringIndizes = [];
    var physicWorld = null;
    var backImage = null;

    $(document).ready(function() {
        $.loadPage(1);
    });

    $.loadPage = function(currentPage)
    {
        context = $('#letterCanvas' + currentPage)[0].getContext('2d');
        canvasSize = {w: context.canvas.width, h: context.canvas.height};
        worldSize = {w: context.canvas.width / SCALE, h: context.canvas.height / SCALE};

        bodies = new Array();
        ringIndizes = new Array();

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
    }

    $(document).ready(function()
    {

    });

    // interaction events
    $(function() 
    {
        var leftButtonDown = false;
        $(document).mousemove(function(e)
        {
            var boundingRect = context.canvas.getBoundingClientRect();
            if(leftButtonDown) {
                physicWorld.updateJointAtMouse({x: e.pageX - boundingRect.left, y: e.pageY - boundingRect.top});
            }
        });

        $(document).bind('touchmove', function(e)
        {
            // see http://www.devinrolsen.com/basic-jquery-touchmove-event-setup/
            e.preventDefault();
            var touch = e.originalEvent.touches[0] || e.originalEvent.changedTouches[0];
            var boundingRect = context.canvas.getBoundingClientRect();
            var touchX = touch.pageX - boundingRect.left;
            var touchY = touch.pageY - boundingRect.top;

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
        physicWorld.updateBodies(bodies, worldSize, jsonData);
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