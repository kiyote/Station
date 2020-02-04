window.render = {
    drawImage: function (canvas, isTransparent, image, x, y) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, x, y);
    },

    drawSprite: function (canvas, isTransparent, image, sx, sy, sw, sh, dx, dy) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, sw, sh);
    },

    drawSpriteScaled: function (canvas, isTransparent, image, sx, sy, sw, sh, dx, dy, dw, dh) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.drawImage(image, sx, sy, sw, sh, dx, dy, dw, dh);
    },

    drawText: function (canvas, isTransparent, font, colour, text, x, y) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.font = font;
        ctx.fillStyle = colour;
        ctx.fillText(text, x, y);
    },

    clear: function (canvas, isTransparent) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.fill();
    },

    fill: function (canvas, isTransparent, colour) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.fillStyle = colour;
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        ctx.fill();
    },

    fillRect: function (canvas, isTransparent, colour, x, y, w, h) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        ctx.beginPath();
        ctx.fillStyle = colour;
        ctx.fillRect(x, y, w, h);
        ctx.fill();
    },

    drawStrokedText: function (canvas, isTransparent, font, colour, text, x, y) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
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
        const ctx = canvas.getContext("2d", { alpha: isTransparent });
        const imageData = ctx.getImageData(sx, sy, sw, sh);
        ctx.putImageData(imageData, x, y);
    },

    renderMapBlock: function (canvas, isTransparent, image, x, y, columns, rows, tileSize, tileColumns, tileRows, tiles) {
        const ctx = canvas.getContext("2d", { alpha: isTransparent });

        let index = 0;
        for (col = 0; col < columns; col++) {
            for (row = 0; row < rows; row++) {
                const tile = tiles[index];
                const tileColumn = tile % tileColumns;
                const tileRow = Math.floor(tile / tileColumns);
                const sx = tileColumn * tileSize;
                const sy = tileRow * tileSize;

                const dx = x + (col * tileSize);
                const dy = y + (row * tileSize);
                ctx.drawImage(image, sx, sy, tileSize, tileSize, dx, dy, tileSize, tileSize);

                index = index + 1;
            }
        }
    }
};