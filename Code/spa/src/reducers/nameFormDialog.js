import { NAME_FORM_DIALOG_OPEN, 
    NAME_FORM_DIALOG_CLOSE, 
    NAME_FORM_DIALOG_SAVING, 
    NAME_FORM_DIALOG_SAVE_SUCCEEDED, 
    NAME_FORM_DIALOG_SAVE_FAILED, 
    } from '../actions/types';


const NameFormDialog = (state = { status: 'closed', errorMessage: null, cancelled: false, data: null, id: null, versionNumber:null, isNew: null, entity: null }, action) => {
    switch (action.type) {
        case NAME_FORM_DIALOG_OPEN: {
            
            if( state.status === 'closed' ){
                
                return Object.assign({}, { ...state }, action.payload );
            }
            
            return state;
        }
        case NAME_FORM_DIALOG_CLOSE:
            
            if( state.status === 'open' || state.status === 'saveSucceeded' ){
                return Object.assign({}, { ...state }, action.payload );
            }

            if( state.status === 'invalid' || state.status === 'saveFailed' ){
                
                if( action.payload.cancelled ){
                    return Object.assign({}, { ...state }, action.payload );
                }
                
                return state;
            }

            return state;
        case NAME_FORM_DIALOG_SAVING:
            
            if( state.status === 'open' || state.status === 'saveFailed'){
                return Object.assign({}, { ...state }, action.payload );
            }

            return state;
        case NAME_FORM_DIALOG_SAVE_SUCCEEDED:
            
            if( state.status === 'saving'){
                return Object.assign({}, { ...state }, action.payload );
            }

            return state;
        case NAME_FORM_DIALOG_SAVE_FAILED:
            
            if( state.status === 'saving'){
                return Object.assign({}, { ...state }, action.payload );
            }

            return state;
        default: 
            return state;
    }
}

export default NameFormDialog;