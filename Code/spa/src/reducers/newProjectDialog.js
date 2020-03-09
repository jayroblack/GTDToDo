import { NEWPROJECTDIALOG_OPEN, NEWPROJECTDIALOG_CLOSE, 
    NEWPROJECTDIALOG_INVALID, NEWPROJECTDIALOG_SAVING, NEWPROJECTDIALOG_SAVE,
    NEWPROJECTDIALOG_SAVE_SUCCEEDED, NEWPROJECTDIALOG_SAVE_FAILED } from '../actions/types';


const NewProjectDialog = (newProjectDialogState = { status: 'closed', errorMessage: null, cancelled: false }, action) => {
    switch (action.type) {
        case NEWPROJECTDIALOG_OPEN: {
            
            if( newProjectDialogState.status === 'closed' ){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'open', errorMessage: null, cancelled: false });
            }
            
            return newProjectDialogState;
        }
        case NEWPROJECTDIALOG_CLOSE:
            
            if( newProjectDialogState.status === 'open' || newProjectDialogState.status === 'saveSuceeded' ){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'closed', errorMessage: null, cancelled: false });
            }

            if( newProjectDialogState.status === 'invalid' || newProjectDialogState.status === 'saveFailed' ){
                //Cannot Close the Dialog if it is in an invalid state unless the use has specifically clicked on the cancel button.
                if( action.payload.cancelled){
                    return Object.assign({}, { ...newProjectDialogState }, { status: 'close', errorMessage: 'Cancelling, no changes were made.', cancelled: false });
                }
                
                return newProjectDialogState;
            }

            return newProjectDialogState;
        case NEWPROJECTDIALOG_INVALID:
            
            if( newProjectDialogState.status === 'open'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'invalid', errorMessage: action.payload.errorMessage, cancelled: false });
            }
            
            return newProjectDialogState;
        case NEWPROJECTDIALOG_SAVING:
            
            if( newProjectDialogState.status === 'open' || newProjectDialogState.status === 'saveFailed'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saving', errorMessage: null, cancelled: false });
            }

            return newProjectDialogState;
        case NEWPROJECTDIALOG_SAVE:
            
            if( newProjectDialogState.status === 'open' || newProjectDialogState.status === 'saveFailed'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saving', errorMessage: null, cancelled: false });
            }

            return newProjectDialogState;
        case NEWPROJECTDIALOG_SAVE_SUCCEEDED:
            
            if( newProjectDialogState.status === 'saving'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saveSuceeded', errorMessage: null, cancelled: false });
            }

            return newProjectDialogState;
        case NEWPROJECTDIALOG_SAVE_FAILED:
            
            if( newProjectDialogState.status === 'saving'){
                return Object.assign({}, { ...newProjectDialogState }, { status: 'saveFailed', errorMessage: action.payload.errorMessage, cancelled: false });
            }
            return newProjectDialogState;
        default: 
            return newProjectDialogState;
    }
}

export default NewProjectDialog;