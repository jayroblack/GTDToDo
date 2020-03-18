import { LOAD_PROJECTS, ADD_PROJECT, DELETED_PROJECT } from '../actions/types';
import _ from 'lodash';

export default (state = { Inbox: {}, Data: {} }, action) => {
    switch ( action.type ){
        case LOAD_PROJECTS: 
            return action.payload;
        case ADD_PROJECT:
            return { ...state, Data: { ...state.Data, [action.payload.id]: action.payload } };
        case DELETED_PROJECT: 
            return { ...state, Data: _.omit(state.Data, action.payload) };
        default:
            return state;
    }
};