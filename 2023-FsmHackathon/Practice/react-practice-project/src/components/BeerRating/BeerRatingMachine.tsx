import { createMachine, assign } from 'xstate';

const getRandomBeer = assign({
  currentBeer: (context: any, event) => {
    let beerIndex = Math.floor(Math.random() * context.beers.length());
    return context.beers[beerIndex];
  }
});

const rateBeer = assign({
  currentBeer: (context:any, event) => {
    
  }
});

const machine = createMachine({
  id: 'cerveza maquinaaaa',
  initial: 'unloaded',
  context: {
    beers: [['Pilsner', 0.0], ['Weihenstephaner', 0.0], ['Julius Echter', 0.0], ['Sapporo', 0.0], ['Kirin', 0.0]],
    currentBeer: []
  },
  states: {
    unloaded: {
      on: {
        APP_START: 'loading'
      }
    },
    loading: {
      on: {
        APP_LOADED: 'loaded'
      }
    },
    loaded: {
      on: {
        BUTTON_PRESSED: {
          actions: 'getRandomBeer',
          target: 'beerOnScreen'
        },
      }
    },
    beerOnScreen: {
      on: {
        BUTTON_PRESSED: {
          actions: 'getRandomBeer',
        },
        RATING_SENT: {
          actions: 
        }
      }
    }
  },
},
{
  actions: { getRandomBeer, rateBeer },
});

/* 
- load beer from list of five predefined beers
- it'll be random
- it'll ask you to rate/validate the poor insecure beer
- next random ass beer (button for next random azz bier)
- enough for now
*/