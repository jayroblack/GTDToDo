import { PROJECT_DIALOG_OPEN, 
    PROJECT_DIALOG_CLOSE, 
    PROJECT_DIALOG_SAVING, 
    PROJECT_DIALOG_SAVE_SUCCEEDED, 
    PROJECT_DIALOG_SAVE_FAILED, 
    } from '../actions/types';


const NewProjectDialog = (newProjectDialogState = { status: 'closed', errorMessage: null, cancelled: false, data: null }, action) => {
    switch (action.type) {
        case PROJECT_DIALOG_OPEN: {
            
            if( newProjectDialogState.status === 'closed' ){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'open' });
            }
            
            return newProjectDialogState;
        }
        case PROJECT_DIALOG_CLOSE:
            
            if( newProjectDialogState.status === 'open' || newProjectDialogState.status === 'saveSucceeded' ){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'closed', errorMessage: null, cancelled: false, data: null });
            }

            if( newProjectDialogState.status === 'invalid' || newProjectDialogState.status === 'saveFailed' ){
                //Cannot Close the Dialog if it is in an invalid state unless the use has specifically clicked on the cancel button.
                if( action.payload.cancelled ){
                    return Object.assign({}, { ...newProjectDialogState }, { status: 'closed', errorMessage: null, cancelled: false, data: null });
                }
                
                return newProjectDialogState;
            }

            return newProjectDialogState;
        case PROJECT_DIALOG_SAVING:
            
            if( newProjectDialogState.status === 'open' || newProjectDialogState.status === 'saveFailed'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saving', errorMessage: null, cancelled: false, data: null });
            }

            return newProjectDialogState;
        case PROJECT_DIALOG_SAVE_SUCCEEDED:
            
            //STOP AUTOMATICALLY CLOSING ON SAVE SUCCEEDED - we need this status, instead invoke the close action optionally based on outcome from the Action.
            if( newProjectDialogState.status === 'saving'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saveSucceeded', errorMessage: null, cancelled: false, data: null });
            }

            return newProjectDialogState;
        case PROJECT_DIALOG_SAVE_FAILED:
            
            if( newProjectDialogState.status === 'saving'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saveFailed', errorMessage: action.payload.errorMessage, cancelled: false, data: null });
            }

            return newProjectDialogState;
        default: 
            return newProjectDialogState;
    }
}

export default NewProjectDialog;