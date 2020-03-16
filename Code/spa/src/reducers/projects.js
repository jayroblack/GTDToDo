import { LOAD_PROJECTS, ADD_PROJECT } from '../actions/types';

export default (state = { Inbox: {}, Data: {} }, action) => {
    switch ( action.type ){
        case LOAD_PROJECTS: 
            return action.payload;
        case ADD_PROJECT:
            var data = state.Data;
            var newData = { ...data, [action.payload.id]: action.payload };
            var myState = { ...state, Data:newData };
            return myState;
        default:
            return state;
    }
};