import { 
    SOBRIETY_PROMPT_DIALOG_OPEN,
    SOBRIETY_PROMPT_DIALOG_DISMISS,
    SOBRIETY_PROMPT_DIALOG_YES,
    SOBRIETY_PROMPT_DIALOG_NO
    } from '../actions/types';

const SobrietyPromptDialog = (state = { status: 'closed'}, action) => {
    switch (action.type) {
        case SOBRIETY_PROMPT_DIALOG_OPEN:{
            return Object.assign({}, { ...state }, action.payload );
        }
        case SOBRIETY_PROMPT_DIALOG_DISMISS:{
            return Object.assign({}, { ...state }, action.payload );
        }
        case SOBRIETY_PROMPT_DIALOG_YES:{
            return Object.assign({}, { ...state }, action.payload );
        }
        case SOBRIETY_PROMPT_DIALOG_NO:{
            return Object.assign({}, { ...state }, action.payload );
        }
        default: 
            return state;
    }
}

export default SobrietyPromptDialog;