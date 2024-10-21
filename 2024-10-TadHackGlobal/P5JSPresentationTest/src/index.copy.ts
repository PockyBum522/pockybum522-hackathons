import p5 from 'p5';
import { createMachine, assign, setup, createActor } from "xstate"
import axios from "axios";

// redo the state machine in TS

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


