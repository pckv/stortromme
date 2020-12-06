window.cfds = {};

window.initializeCanvas = (canvas, displayData) => {
    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: canvas.id,
        width: 500,
        height: 500,
        lineWidth: getLineWidth(),
        strokeColor: getColor(),
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
    console.log('saving canvas ' + canvas.id);
    return window.cfds[canvas.id].save();
}

window.disposeCanvas = (canvas) => {
    delete window.cfds[canvas.id];
    delete localStorage.canvasCache;
}

function createButtons(canvas, cfd) {
    createLineWidthButton(canvas, cfd);
    createButton(canvas, 'Fill', () => cfd.toggleBucketTool());

    createButton(canvas, 'Redo', () => cfd.redo());
    createButton(canvas, 'Undo', () => cfd.undo());
    createButton(canvas, 'Clear', () => cfd.clear());

    createColorButtons(canvas, cfd);
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

function createColorButtons(canvas, cfd) {
    // color palette from PICO 8: https://lospec.com/palette-list/pico-8
    colors = [
        [0,0,0],        [29,43,83],     [126,37,83],    [0,135,81],
        [171,82,54],    [95,87,79],     [194,195,199],  [255,241,232],
        [255,0,77],     [255,163,0],    [255,236,39],   [0,228,54],
        [41,173,255],   [131,118,156],  [255,119,168],  [255,204,170],
    ];

    const colorButtonContainer = document.createElement('div');
    for (const button of colors.map(c => createColorButton(c, cfd))) {
        colorButtonContainer.appendChild(button);
    }

    canvas.parentElement.append(colorButtonContainer);
}

function createColorButton(color, cfd) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-outline-dark');
    button.style.backgroundColor = `rgb(${color.join(',')})`
    button.onclick = () => setColor(color, cfd);
    return button;
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

function setColor(color, cfd) {
    localStorage.canvasColor = JSON.stringify(color);
    cfd.setDrawingColor(color);
}

function getColor() {
    return localStorage.canvasColor
        ? JSON.parse(localStorage.canvasColor)
        : [0, 0, 0];
}
