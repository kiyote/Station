window.render = {
    drawImage: function (canvas, isTransparent, image, x, y) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, x, y);
    },

    drawSprite: function (canvas, isTransparent, image, sx, sy, sw, sh, dx, dy) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, sw, sh);
    },

    drawSpriteScaled: function (canvas, isTransparent, image, sx, sy, sw, sh, dx, dy, dw, dh) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, dw, dh);
    },

    drawText: function (canvas, isTransparent, font, colour, text, x, y) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.font = font;
        ctx.fillStyle = colour;
        ctx.fillText(text, x, y);
    },

    clear: function (canvas, isTransparent) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.fill();
    },

    fill: function (canvas, isTransparent, colour) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.fillStyle = colour;
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.fill();
    },

    fillRect: function (canvas, isTransparent, colour, x, y, w, h) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.fillStyle = colour;
        ctx.fillRect(x, y, w, h);
        ctx.fill();
    },

    drawStrokedText: function (canvas, isTransparent, font, colour, text, x, y) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.fillStyle = colour;
        ctx.font = font;
        ctx.strokeStyle = "black";
        ctx.lineWidth = 2;
        ctx.lineJoin = "round";
        ctx.miterLimit = 2;
        ctx.strokeText(text, x, y);
        ctx.fillText(text, x, y);
    },

    copyRect: function (canvas, isTransparent, sx, sy, sw, sh, x, y) {
        var ctx = canvas.getContext("2d", { alpha: isTransparent });
        var imageData = ctx.getImageData(sx, sy, sw, sh);
        ctx.putImageData(imageData, x, y);
    }
};