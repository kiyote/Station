window.anim = {
    start: function (instance) {
        return window.requestAnimationFrame(function (timestamp) { anim.callback(instance); });
    },

    callback: function (instance) {
        var frameTime = performance.now();
        instance.invokeMethodAsync("AnimCallback", frameTime);
        var callbackId = window.requestAnimationFrame(function (timestamp) { anim.callback(instance); });
        instance.invokeMethodAsync("SetCallbackContext", callbackId);
    },

    stop: function (callbackId) {
        window.cancelAnimationFrame(callbackId);
    }
};