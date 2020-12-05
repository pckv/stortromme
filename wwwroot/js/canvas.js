window.cfds = {};

window.initializeCanvas = (canvas, displayData) => {
    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: canvas.id,
        width: 500,
        height: 500,
        lineWidth: getLineWidth(),
        maxSnapshots: 30,
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

    // create all interactive elements
    createButtons(canvas, cfd);

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

function createButtons(canvas, cfd) {
    createLineWidthButton(canvas, cfd);

    createButton(canvas, 'Redo', () => cfd.redo());
    createButton(canvas, 'Undo', () => cfd.undo());
    createButton(canvas, 'Clear', () => cfd.clear());
}

function createButton(canvas, name, action) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-secondary');
    button.onclick = action;
    button.innerText = name;
    canvas.parentElement.prepend(button);
}

function createLineWidthButton(canvas, cfd) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-outline-dark');

    // Cycle through 2px, 5px, 8px line widths
    button.onclick = () => {
        let width = getLineWidth();
        width = width < 8 ? width + 3 : 2;

        setLineWidth(width, cfd)
        button.innerText = getLineWidth();
    }
    button.innerText = getLineWidth();

    canvas.parentElement.prepend(button);
}

function setLineWidth(lineWidth, cfd) {
    localStorage.canvasLineWidth = lineWidth;
    cfd.setLineWidth(lineWidth);
}

function getLineWidth() {
    return localStorage.canvasLineWidth
        ? parseInt(localStorage.canvasLineWidth)
        : 2;
}
