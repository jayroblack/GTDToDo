import { NAME_FORM_DIALOG_OPEN, 
    NAME_FORM_DIALOG_CLOSE, 
    NAME_FORM_DIALOG_SAVING, 
    NAME_FORM_DIALOG_SAVE_SUCCEEDED, 
    NAME_FORM_DIALOG_SAVE_FAILED, 
    } from '../actions/types';


const NameFormDialog = (state = { status: 'closed', errorMessage: null, cancelled: false, data: null, id: null, versionNumber:null, isNew: null }, action) => {
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
                //Cannot Close the Dialog if it is in an invalid state unless the use has specifically clicked on the cancel button.
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
            
            //STOP AUTOMATICALLY CLOSING ON SAVE SUCCEEDED - we need this status, instead invoke the close action optionally based on outcome from the Action.
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