import { LOAD_PROJECTS, ADD_PROJECT } from '../actions/types';

export default (state = { }, action) => {
    switch ( action.type ){
        case LOAD_PROJECTS: 
            return action.payload;
        case ADD_PROJECT:
            return { ...state, [action.payload.id]: action.payload };
        default:
            return state;
    }
};