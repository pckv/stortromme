window.cfds = {};

// color palette from PICO 8: https://lospec.com/palette-list/pico-8
// also includes a 17th pure white color
const colors = [
    [0,0,0],        [29,43,83],     [126,37,83],    [0,135,81],
    [171,82,54],    [95,87,79],     [194,195,199],  [255,241,232],
    [255,0,77],     [255,163,0],    [255,236,39],   [0,228,54],
    [41,173,255],   [131,118,156],  [255,119,168],  [255,204,170],
    [255, 255, 255],
];

window.initializeCanvas = (canvas, displayData) => {
    // initialize
    const cfd = new CanvasFreeDrawing.default({
        elementId: canvas.id,
        width: 700,
        height: 700,
        lineWidth: getLineWidth(),
        strokeColor: getColor(),
        backgroundColor: colors[16],  // white background from palette
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
    const tools = document.createElement('div');
    tools.classList.add('canvas-tools');
    canvas.parentElement.prepend(tools);

    const rightTools = document.createElement('div');
    const leftTools = document.createElement('div');
    tools.prepend(rightTools);
    tools.prepend(leftTools);

    const lineWidthButton = createLineWidthButton(rightTools, cfd);
    createIconButton(rightTools, 'format_color_fill', button => {
        const state = cfd.toggleBucketTool();
        if (!state) {
            button.classList.remove('btn-dark');
            button.classList.add('btn-outline-dark');
        } else {
            button.classList.remove('btn-outline-dark');
            button.classList.add('btn-dark');
        }
    });
    
    createIconButton(leftTools, 'redo', () => cfd.redo());
    createIconButton(leftTools, 'undo', () => cfd.undo());
    createIconButton(leftTools, 'delete', () => cfd.clear());

    createColorButtons(canvas, cfd, lineWidthButton);
}

function createIconButton(parent, icon, action) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-outline-dark', 'icon-button');
    button.onclick = () => action(button);
    button.innerHTML = `<span class="material-icons canvas-icon">${icon}</span>`;
    parent.prepend(button);
}

function createLineWidthButton(parent, cfd) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-outline-dark', 'icon-button');
    
    const dot = document.createElement('span');
    dot.classList.add('dot');
    button.appendChild(dot);

    button.drawShape = () => {
        const dotRadius = `${getLineWidth()}px`;
        dot.style.width = dotRadius;
        dot.style.height = dotRadius;
        dot.style.backgroundColor = `rgb(${getColor().join(',')})`;
    }

    // Cycle through 2px, 5px, 8px line widths
    button.onclick = () => {
        let width = getLineWidth();
        width = width < 8 ? width + 3 : 2;

        setLineWidth(width, cfd);
        button.drawShape();
    }

    button.drawShape();

    parent.prepend(button);
    return button;
}

function createColorButtons(canvas, cfd, lineWidthButton) {
    const colorButtonContainer = document.createElement('div');
    colorButtonContainer.classList.add('canvas-color-button-container');
    for (const button of colors.map(c => createColorButton(c, cfd, lineWidthButton))) {
        colorButtonContainer.appendChild(button);
    }

    canvas.parentElement.append(colorButtonContainer);
}

function createColorButton(color, cfd, lineWidthButton) {
    const button = document.createElement('button');
    button.classList.add('btn', 'btn-outline-dark', 'canvas-color-button');
    button.style.backgroundColor = `rgb(${color.join(',')})`
    button.onclick = () => {
        setColor(color, cfd);
        lineWidthButton.drawShape();
    };
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
