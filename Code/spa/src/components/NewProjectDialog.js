import React from 'react';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, DialogContentText, DialogActions, Button, TextField, LinearProgress } from '@material-ui/core'
import { CloseNewProjectDialog, SaveNewProjectDialog } from '../actions/newProjectDialog';

const useStyles = theme => ({

});

class NewProjectDialog extends React.Component {
    
    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseNewProjectDialog(cancelled));
    };

    onSubmit = (formValues) => {
        this.props.dispatch(SaveNewProjectDialog(this.props.userProfile.access_token, formValues.projectName));
        //When Save Returns Successful - Close Dialog - How can I show that it worked?  Show Snack Bar Success?  
        //When Save Returns Failed throw new SubmissionError({ projectName: 'Message.', _error: 'Save Failed.' })
    };

    renderTextField = ({ input, label, meta, ...custom }) => {
        const opts = {};
        if( meta.error && meta.touched){
            opts["error"] = true;
            opts["helperText"] = meta.error;
        }
        
        return (
        <TextField autoFocus
            required 
            {... opts}
            label={label}
            margin="dense"
            fullWidth
          {...input}
          {...custom}
        />
        );
    }

    render() {
        let disabledOptions = {};

        if( this.props.newProjectDialogState.status === 'saving' ){
            disabledOptions = { disabled:true };
        }
        else{
            disabledOptions = { };
        }

        return (
            <form name="newProjectDialog" >
            <Dialog open={this.props.newProjectDialogState.status !== 'closed'} onClose={() => this.handleDismiss(false)} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Add New Project</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Enter the new project name and click save.
                    </DialogContentText>
                    <Field name="projectName" label="Project Name" component={this.renderTextField} type="text" {...disabledOptions} />
                </DialogContent>
                <DialogActions>
                    <Button variant="contained" onClick={ ()=> this.handleDismiss(true)} color="secondary" {...disabledOptions}>
                        Cancel
                    </Button>
                    <Button variant="contained" onClick={this.props.handleSubmit(this.onSubmit)} color="primary" {...disabledOptions}>
                        Save
                    </Button>
                </DialogActions>
                {this.props.newProjectDialogState.status === 'saving' && <LinearProgress />}
            </Dialog>
            </form>
        );
    }
}

const validate = (formValues) => {
    const errors = {};

    if( !formValues.projectName){
        errors.projectName = "Project name is required."
        return errors;
    }

    if( formValues.projectName === 'Inbox'){
        errors.projectName = "Inbox is a reserved project name."
        return errors;
    }
    
    //TODO:  I would love to be able to verify that the projects do not already exist before sending to server.
    //Problem is that I cannot register this external validation function and have it be aware of my map state to props. :(

    return errors;
}

NewProjectDialog = reduxForm({
    form: 'newProjectDialog',
    validate
})(NewProjectDialog)

NewProjectDialog = connect(
    state => {
      return { 
          projects: state.labelsAndProjects.data.projects, 
          newProjectDialogState: state.newProjectDialog,
          myForm: state.form.newProjectDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(NewProjectDialog)


export default (withStyles(useStyles))(NewProjectDialog);