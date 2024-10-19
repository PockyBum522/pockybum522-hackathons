import p5 from 'p5';
import { CanvasCapture } from "canvas-capture";

const frameRate = 60;

let r = 0;
const x = 300;
const y = 200;
let theta = 0;
let alpha = 0;

let canvasId;

let isRecording = false;

window.onload = function() {
    console.log("Window Loaded");
}

function postSetup() {
    canvasId = document.getElementById("defaultCanvas0");
    CanvasCapture.init(canvasId, { showRecDot: true, verbose: true });

    console.log(canvasId);
}

function beginGifRecord() {
    CanvasCapture.beginGIFRecord({ name: 'myGIF', fps: 10 });
}

function stopGifRecord() {
    CanvasCapture.stopRecord().then(() => {
        console.log("Recording stopped");
    });
}

new p5((p) => {

    p.setup = () => {
        p.frameRate = frameRate;
        p.createCanvas(400, 400);

        let startRecordingButton = p.createButton('Start Recording');
        let stopRecordingButton = p.createButton('STAHP Recording');

        startRecordingButton.position(0, 0);
        startRecordingButton.mousePressed(beginGifRecord);

        stopRecordingButton.position(0, 20);
        stopRecordingButton.mousePressed(stopGifRecord);

        postSetup();


    };

    p.draw = () => {

        p.background(220);
        p.ellipse(x*Math.sin(theta), y*Math.cos(alpha), 50, r);

        theta -= .1;
        alpha += .01;
        r += 1;

        if (CanvasCapture.isRecording()) {
            CanvasCapture.recordFrame();
        }
    };
});