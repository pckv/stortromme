window.initializeCanvas = (canvas) => {
    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: 'cfd',
        width: 500,
        height: 500,
    });

    // set properties
    cfd.setLineWidth(10); // in px
    cfd.setStrokeColor([0, 0, 255]); // in RGB

    // listen to events
    cfd.on({ event: 'redraw' }, () => {
        // code...
    });
}