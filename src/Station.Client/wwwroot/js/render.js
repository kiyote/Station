window.render = {
    drawImage: function (canvas, image, x, y) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.drawImage(image, x, y);
    },

    drawSprite: function (canvas, image, sx, sy, sw, sh, dx, dy) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, sw, sh);
    },

    drawSpriteScaled: function (canvas, image, sx, sy, sw, sh, dx, dy, dw, dh) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, dw, dh);
    },

    drawText: function (canvas, text, font, x, y) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.font = font;
        ctx.fillText(text, x, y);
    },

    clear: function (canvas) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    },

    fill: function (canvas, colour) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.save();
        ctx.fillStyle = colour;
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.restore();
    },

    fillRect: function (canvas, colour, x, y, w, h) {
        var ctx = canvas.getContext("2d", { alpha: false });
        ctx.save();
        ctx.fillStyle = colour;
        ctx.fillRect(x, y, w, h);
        ctx.restore();
    }
};