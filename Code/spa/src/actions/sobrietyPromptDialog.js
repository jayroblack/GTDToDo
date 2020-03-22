import { 
    SOBRIETY_PROMPT_DIALOG_OPEN,
    SOBRIETY_PROMPT_DIALOG_DISMISS,
    SOBRIETY_PROMPT_DIALOG_YES,
    SOBRIETY_PROMPT_DIALOG_NO
    } from './types';

export const OpenSobrietyPrompt = (title, question, data) =>{
    return {
        type: SOBRIETY_PROMPT_DIALOG_OPEN, 
        payload: { 
            status: 'open',
            result: null,
            title,
            question,
            data
        }
    }
}

export const DismissSobrietyPrompt = () =>{
    return {
        type: SOBRIETY_PROMPT_DIALOG_DISMISS, 
        payload: { 
            status: 'closed',
            result: 'dismiss',
            title: '',
            question: ''
        }
    }
}

export const AffirmativeSobrietyPrompt = () =>{
    return {
        type: SOBRIETY_PROMPT_DIALOG_YES, 
        payload: { 
            status: 'closed',
            result: 'yes',
            title: '',
            question: ''
        }
    }
}

export const NegativeSobrietyPrompt = () =>{
    return {
        type: SOBRIETY_PROMPT_DIALOG_NO, 
        payload: { 
            status: 'closed',
            result: 'no',
            title: '',
            question: ''
        }
    }
}