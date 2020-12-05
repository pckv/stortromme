window.cfds = {};

window.initializeCanvas = (canvas, displayData) => {
    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: canvas.id,
        width: 500,
        height: 500,
        lineWidth: 8,
        disabled: !!displayData,
    });

    // add the canvas to global scope
    window.cfds[canvas.id] = cfd;

    // only display data when this parameter is specified
    if (displayData) {
        cfd.restore(displayData);
        return;
    }

    if (localStorage.canvasCache) {
        cfd.restore(localStorage.canvasCache);
    }

    const container = document.getElementById(`${canvas.id}-container`);
    // store local snapshot every 10 redraws
    // todo maybe: store on every mouseup/touchend/clear (or fork the repo and
    // add an event for every complete action, including undo/redo)
    cfd.on({ event: 'redraw', counter: 10 }, () => {
        localStorage.canvasCache = cfd.save();
    });
}

window.saveCanvas = (canvas) => {
    return window.cfds[canvas.id].save();
}

window.disposeCanvas = (canvas) => {
    delete window.cfds[canvas.id];
    delete localStorage.canvasCache;
}
