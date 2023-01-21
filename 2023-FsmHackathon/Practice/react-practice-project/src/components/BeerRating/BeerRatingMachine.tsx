import { createMachine, assign } from 'xstate';

const getRandomBeer = assign({
  currentBeer: (context: any, event) => {
    let beerIndex = Math.floor(Math.random() * context.beers.length());
    return context.beers[beerIndex];
  }
});

// const rateBeer = assign({
//   currentBeer: (context:any, event) => {
//     context.currentBeer[1] = event.data.rating;
//   }
// });

const machine = createMachine({
  id: 'cerveza maquinaaaa',
  schema: {
    context: {} as 
        { beers: any } 
      | { currentBeer: any },
    events: {} as
      | { type: 'APP_START' }
      | { type: 'APP_LOADED' }
      | { type: 'BUTTON_PRESSED' }
      | { type: 'RATING_SENT'; rating: number }

  },
  initial: 'unloaded',
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
          actions: 'rateBeer'
        }
      }
    }
  },
},
{
  actions: { 
    getRandomBeer: (context, event) => {
      console.log(event.rating);
    } 
  },
});

/* 
- load beer from list of five predefined beers
- it'll be random
- it'll ask you to rate/validate the poor insecure beer
- next random ass beer (button for next random azz bier)
- enough for now
*/