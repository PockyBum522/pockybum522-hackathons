import p5 from 'p5';
import { createMachine, assign, createActor } from "xstate"
import axios from "axios";
import './styles.css';
import './normalize.css';

const sceneTimings = {
    'SCENE_ONE': 86400,
    'SCENE_TWO': 1,
    'SCENE_THREE': 2,
    'SCENE_FOUR': 3,
    'SCENE_FIVE': 86400,
    'SCENE_SIX': 5,
    'SCENE_SEVEN': 6,
    'SCENE_EIGHT': 7,
    'DEFAULT': 8,
}

window.onload = function() {
    console.log("Window Loaded");
}

const sceneMachine = createMachine(
    {
        context: {
            sceneNumber: 1,
        },
        on: {
            ADVANCE: {
                actions: "assignToScene",
            },
            BACKTRACK: {
                actions: "assignToScene",
            },
            REWIND: {
                actions: "assignToScene",
            }
        },
    },
    {
    actions: {
        assignToScene: assign({
                sceneNumber: ({ context, event }) => context.sceneNumber + event.advanceBy,
        })
    }
});

const sceneActor = createActor(sceneMachine).start();

sceneActor.subscribe((state) => {

    let sceneTiming = 0;

    switch(state.context.sceneNumber) {
        case 1:
            sceneTiming = sceneTimings.SCENE_ONE;
            break;
        case 2:
            sceneTiming = sceneTimings.SCENE_TWO;
            break;
        case 3:
            sceneTiming = sceneTimings.SCENE_THREE;
            break;
        case 4:
            sceneTiming = sceneTimings.SCENE_FOUR;
            break;
        case 5:
            sceneTiming = sceneTimings.SCENE_FIVE;
            break;
        case 6:
            sceneTiming = sceneTimings.SCENE_SIX;
            break;
        case 7:
            sceneTiming = sceneTimings.SCENE_SEVEN;
            break;
        case 8:
            sceneTiming = sceneTimings.SCENE_EIGHT;
            break;
        default:
            sceneTiming = sceneTimings.DEFAULT;
            break;
    }

    console.log(`Current scene number: ${state.context.sceneNumber}`);

    if (sceneTimers[state.context.sceneNumber - 1] != null) {
        sceneTimers[state.context.sceneNumber - 1].clearTimeout();
        sceneTimers[state.context.sceneNumber - 1].setTimeout(() => {
            sceneActor.send( {
                type: 'ADVANCE',
                advanceBy: 1
            });
        }, sceneTiming * 1000);
    } else {
        // if the previous scene timer is still running, kill the timer
        if (sceneTimers[state.context.sceneNumber - 2] != null) {
            sceneTimers[state.context.sceneNumber - 2].clearTimeout();
        }
        sceneTimers.push(setTimeout(() => {
            sceneActor.send( {
                type: 'ADVANCE',
                advanceBy: 1
            });
        }, sceneTiming * 1000));
    }
});

let theta = Math.PI;
let r = 50;
let x_pos = 100;
let y_pos = 100;
let offset = 0;

// For Ed Shear-en
let i = 0;

let countDownTime = 30;

const maxScenes = 8;

let checkServerTimeout;
const serverCheckIntervalTime = 250;

let lastEventsList = new Set();
let newEventsList = new Set();
let lastTimestampsList = new Set();
let newTimestampsList = new Set();

// scenes 1 and 5 have no associated timer
let sceneTimers = [];

function findVconsInEvents(events) {
    //console.log(events);
    let vCons = [];

    events.forEach(event => {
        if (event.Type === "VconWithSentiment") {
            // console.log("vCon detec-a-ted");
            vCons.push(event);
        }
    });

    return vCons;
}

new p5((p) => {

    p.keyPressed = () => {
        if (p.key === ' ') {
            if (sceneActor.getSnapshot().context.sceneNumber >= maxScenes) {
                sceneActor.send( {
                    type: 'REWIND',
                    advanceBy: -maxScenes
                });
            }
            sceneActor.send( {
                type: 'ADVANCE',
                advanceBy: 1
            });
        }

        if (p.keyCode === 37) {
            sceneActor.send({
                type: 'BACKTRACK',
                advanceBy: -1
            });
        }

    }

    p.windowResized = () => {
        p.resizeCanvas(p.windowWidth, p.windowHeight);
    }

    p.setup = async () => {
        p.createCanvas(p.windowWidth, p.windowHeight);

        checkServerTimeout = setInterval(async() => {
            const response = await axios.get("http://192.168.1.24:5001/", {
                headers: {
                    'Access-Control-Allow-Origin': "*",
                    'Content-Type': 'application/json;charset=utf-8',
                }
            });

            newEventsList = new Set(response.data);

            // create timestamps set from new events
            newEventsList.forEach(event => {
               newTimestampsList.add(event.Timestamp);
            });

            // create timestamps set from last events
            lastEventsList.forEach(event => {
                lastTimestampsList.add(event.Timestamp);
            })

            // console.log(newTimestampsList);
            // console.log(lastTimestampsList);
            console.log(lastEventsList);
            console.log(newEventsList);

            if (lastTimestampsList.difference(newTimestampsList).size < newTimestampsList.size) {
                console.log("out of sync; update lastEventsList");
                lastEventsList = new Set([...newEventsList]);
            }

        }, serverCheckIntervalTime);

        console.log(sceneActor.getSnapshot().context.sceneNumber);
    };

    p.draw = () => {
        p.background(220);
        let sceneNumber = sceneActor.getSnapshot().context.sceneNumber;

        let vCons = findVconsInEvents(newEventsList);

        p.push();
            p.textSize(30);
            p.text(`Scene ${sceneActor.getSnapshot().context.sceneNumber}`, 10, 30);
        p.pop();

        if (sceneNumber === 1) {
            p.push();
                p.textSize(50);
                p.text("This scene will contain a soothing ambience", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 2) {
            p.push();
                p.textSize(50);
                p.text("This scene will contain an energized ambience", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 3) {
            p.push();
                p.textSize(50);
                p.text("This scene will fade out over the course of 30 seconds", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 4) {
            p.push();
                p.textSize(50);
                p.text("This scene will contain words that appear indistinctly", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 5) {
            p.push();
                p.textSize(50);
                p.text("This scene will contain a white circle that appears in center of screen", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 6) {
            p.push();
                p.textSize(50);
                p.text("Spoken words appear in random places on screen and begin swirling into circle", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 7) {
            p.push();
                p.textSize(50);
                p.text("Prompt to take coin out into world", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }

        if (sceneNumber === 8) {
            p.push();
                p.textSize(50);
                p.text("Scene fades to nothing", p.windowWidth/2, p.windowHeight/2);
            p.pop();
        }
    };
});