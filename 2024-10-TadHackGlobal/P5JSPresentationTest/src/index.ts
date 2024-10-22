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
        context: {} as { sceneCount: number, changeAmount: number },
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

const maxScenes:number = 8;

for (let i = 0; i < maxScenes; i++) {
    sceneTimers.push(0);
}

function clearAllSceneTimers() {
    for (let i = 0; i < maxScenes; i++) {
        clearTimeout(sceneTimers[i]);
    }
}

new p5((p) => {


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

        //testing scene timers

        sceneTimers[0] = window.setTimeout(() => { console.log(`sceneTimer 0 fires`)}, 1000);
        sceneTimers[1] = window.setTimeout(() => { console.log(`sceneTimer 1 fires`)}, 2000);
        sceneTimers[2] = window.setTimeout(() => { clearAllSceneTimers() }, 3000);
        sceneTimers[3] = window.setTimeout(() => { console.log(`sceneTimer 3 fires`)}, 4000);
        sceneTimers[4] = window.setTimeout(() => { console.log(`sceneTimer 4 fires`)}, 5000);
        sceneTimers[5] = window.setTimeout(() => { console.log(`sceneTimer 5 fires`)}, 6000);
        sceneTimers[6] = window.setTimeout(() => { console.log(`sceneTimer 6 fires`)}, 7000);
    };

    p.draw = () => {
        p.background(155);
    };
});

// ----***** END *****-----