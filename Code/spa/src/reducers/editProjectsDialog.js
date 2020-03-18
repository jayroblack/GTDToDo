import { EDIT_PROJECTS_DIALOG_OPEN, EDIT_PROJECTS_DIALOG_CLOSE,
EDIT_PROJECTS_DIALOG_PENDING_RESPONSE, EDIT_PROJECTS_DIALOG_RESPONSE_SUCCEEDED,
EDIT_PROJECTS_DIALOG_RESPONSE_FAILED } from '../actions/types';

const EditProjectDialog = (state = { status: 'closed' }, action) => {
    switch (action.type) {
        case EDIT_PROJECTS_DIALOG_OPEN:{
            if( state.status === 'closed' ){
                return Object.assign({}, { ...state }, action.payload );
            }
            return state;
        }
        case EDIT_PROJECTS_DIALOG_CLOSE:{
            if( state.status !== 'pending' ){
                return Object.assign({}, { ...state }, action.payload );
            }
            return state;
        }
        case EDIT_PROJECTS_DIALOG_PENDING_RESPONSE:{
            return Object.assign({}, { ...state }, action.payload );
        }
        case EDIT_PROJECTS_DIALOG_RESPONSE_SUCCEEDED:{
            return Object.assign({}, { ...state }, action.payload );
        }
        case EDIT_PROJECTS_DIALOG_RESPONSE_FAILED: {
            return Object.assign({}, { ...state }, action.payload );
        }
        default: 
            return state;
    }
}

export default EditProjectDialog;