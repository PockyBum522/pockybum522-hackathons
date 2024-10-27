import p5 from 'p5';
import { createMachine, assign, setup, createActor } from "xstate"
import axios from "axios";

//TODO: refactor sections into different files

//TODO: Also refactor the state machine into its own class
// ----***** State Machine Section *****-----

let sceneTimers: Array<number> = [];

interface SceneContext {
    sceneCount: number;
    changeAmount: number;
}

interface SceneEvents {
    type: 'CHANGE_SCENE';
}

const sceneMachine = setup({
    types: {
        context: {} as SceneContext,
        events: {} as
            | { type: 'CHANGE_SCENE'; changeAmount: number; }
            | { type: 'REWIND_PRESENTATION', changeAmount: number }
    },
    actions: {
        increment: assign({
            sceneCount: ({ context, event }) => context.sceneCount + event.changeAmount
        }),
        rewind: assign({
            sceneCount: ({ context }) => context.sceneCount = 0
        })
    }
}).createMachine({
    context: { sceneCount: 0, changeAmount: 0 },
    on: {
        "CHANGE_SCENE": { actions: 'increment' },
        "REWIND_PRESENTATION": { actions: 'rewind' }
    }
});



const sceneActor = createActor(sceneMachine).start();

sceneActor.subscribe((state) => {
    console.log(state.context.sceneCount);
});


// ----***** END *****-----


// ----***** P5JS Section *****-----

// TODO: Refactor this into a class

let checkServerTimeout: NodeJS.Timeout | null = null;
let newEventsList:Set<string> = new Set();
let lastEventsList:Set<string> = new Set();

const maxScenes:number = 9;

for (let i = 0; i < maxScenes; i++) {
    sceneTimers.push(0);
}

function clearAllSceneTimers() {
    for (let i = 0; i < maxScenes; i++) {
        clearTimeout(sceneTimers[i]);
    }
}

new p5((p) => {

    const ws = new WebSocket('ws://127.0.0.1:5000/ws');

    ws.onopen = ((event:Event) => {
        console.log('Connected to server');

        ws.send('Hello, server!');
    });

    ws.onmessage = ((message:MessageEvent) => {
        console.log(`Received message from server: ${JSON.stringify(message.data)}`);
        let msg = JSON.stringify(message.data);
        console.log(msg);
    });

    ws.onclose = ((close:CloseEvent) => {
        console.log('Disconnected from server');
    });

    p.keyPressed = () => {

        let sceneCount = sceneActor.getSnapshot().context.sceneCount;

        if (p.key === ' ' && sceneCount >= maxScenes) {
            console.log("rewind to beginning");
            sceneActor.send({
                type: 'REWIND_PRESENTATION',
                changeAmount: 0
            });
        }

        if (p.key === ' ' && sceneCount >= 0 && sceneCount < maxScenes) {
            console.log("advance a scene");
            sceneActor.send({
                type: 'CHANGE_SCENE',
                changeAmount: 1
            });
        }

        if (p.keyCode === 37 && sceneCount > 0) {
            sceneActor.send({
                type: 'CHANGE_SCENE',
                changeAmount: -1
            });
        }
    }

    p.setup = () => {
        p.createCanvas(200, 200);


    };

    p.draw = () => {
        p.background(155);
    };
});

// ----***** END *****-----