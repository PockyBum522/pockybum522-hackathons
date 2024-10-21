import p5 from 'p5';
import { createMachine, assign, setup, createActor } from "xstate"
import axios from "axios";

//TODO: refactor sections into different files

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
        context: {} as { sceneCount: number },
        events: {} as
            | { changeAmount: number; type: 'CHANGE_SCENE' }
    },
    actions: {
        increment: assign({
            sceneCount: ({ context, event }) => context.sceneCount + event.changeAmount
        })
    }
}).createMachine({
    context: { sceneCount: 0 },
    on: {
        "CHANGE_SCENE": { actions: 'increment' },
    }
});



const sceneActor = createActor(sceneMachine).start();

sceneActor.subscribe((state) => {

});


// ----***** END *****-----


// ----***** P5JS Section *****-----

const maxScenes:number = 8;

for (let i = 0; i < maxScenes; i++) {
    sceneTimers.push(0);
}

// ----***** END *****-----