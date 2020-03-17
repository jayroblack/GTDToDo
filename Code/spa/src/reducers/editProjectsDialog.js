import {EDIT_PROJECTS_DIALOG_OPEN, EDIT_PROJECTS_DIALOG_CLOSE} from '../actions/types';

const EditProjectDialog = (state = { status: 'closed' }, action) => {
    switch (action.type) {
        case EDIT_PROJECTS_DIALOG_OPEN:{
            if( state.status === 'closed' ){
                return Object.assign({}, { ...state }, { status: 'open' });
            }
            return state;
        }
        case EDIT_PROJECTS_DIALOG_CLOSE:{
            if( state.status === 'open' ){
                return Object.assign({}, { ...state }, { status: 'closed' });
            }
            return state;
        }
        default: 
            return state;
    }
}

export default EditProjectDialog;