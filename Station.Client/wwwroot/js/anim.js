window.anim = {
    start: function (instance) {
        return requestAnimationFrame(function(timestamp) { anim.callback(timestamp, instance); });
    },

    callback: function (timestamp, instance) {
        var interval = 16;

        instance.invokeMethodAsync("AnimCallback", interval);
        var callbackId = requestAnimationFrame(function(timestamp) { anim.callback(timestamp, instance); });
        instance.invokeMethodAsync("SetCallbackContext", callbackId);
    },

    stop: function (callbackId) {
        cancelAnimationFrame(callbackId);
    }
};