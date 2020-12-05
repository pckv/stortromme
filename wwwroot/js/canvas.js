window.cfds = {};

window.initializeCanvas = (canvas, displayData) => {
    console.log('initializing canvas ' + canvas.id);

    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: canvas.id,
        width: 500,
        height: 500,
        disabled: !!displayData,
    });

    // add the canvas to global scope
    window.cfds[canvas.id] = cfd;

    // only display data when this parameter is specified
    if (displayData) {
        cfd.restore(displayData);
        return;
    }

    // set properties
    cfd.setLineWidth(8); // in px
    cfd.setStrokeColor([0, 0, 0]); // in RGB

    // listen to events
    cfd.on({ event: 'redraw' }, () => {
        // code...
    });
}

window.saveCanvas = (canvas) => {
    console.log('saving canvas ' + canvas.id);
    return window.cfds[canvas.id].save();
}

window.disposeCanvas = (canvas) => {
    console.log('disposing canvas ' + canvas.id);
    delete window.cfds[canvas.id]
}
