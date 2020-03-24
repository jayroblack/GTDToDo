import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, 
    DialogContentText, DialogActions, Button, TextField, LinearProgress } from '@material-ui/core'
import { CloseProjectDialog } from '../actions/projectDialog';
import { withSnackbar } from 'notistack';
import { FORM_PROJECT_DIALOG } from '../forms'

const useStyles = theme => ({

});

class ProjectFormDialog extends React.Component {

    constructor(props){
        super(props);
        this.timer = null;
    }

    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseProjectDialog(cancelled));
    };

    componentWillUnmount(){
        clearTimeout(this.timer);
    }

    componentDidUpdate(prevProps){
        if( this.props.componentDidUpdateCallback && prevProps && this.props ){
            this.props.componentDidUpdateCallback(prevProps, this.props);
        }
    }

    renderTextField = ({ input, label, meta, ...custom }) => {
        const opts = {};
        if( meta.error && meta.touched){
            opts["error"] = true;
            opts["helperText"] = meta.error;
        }
        
        if( this.props.projectDialogState.status === 'saveFailed' ){
            opts["error"] = true;
            opts["helperText"] = this.props.projectDialogState.errorMessage;
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

        if( this.props.projectDialogState.status === 'saving' ){
            disabledOptions = { disabled:true };
        }
        else{
            disabledOptions = { };
        }
        
        return (
            <form name={FORM_PROJECT_DIALOG} >
            <Dialog open={this.props.projectDialogState.status !== 'closed'} onClose={() => this.handleDismiss(false)} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">{this.props.title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {this.props.description}
                    </DialogContentText>
                    <Field name="name" label="Project Name" component={this.renderTextField} type="text" {...disabledOptions} />
                </DialogContent>
                <DialogActions>
                    <Button variant="contained" onClick={ ()=> this.handleDismiss(true)} color="secondary" {...disabledOptions}>
                        Cancel
                    </Button>
                    <Button variant="contained" onClick={this.props.handleSubmit(this.props.onSubmit)} color="primary" {...disabledOptions}>
                        Save
                    </Button>
                </DialogActions>
                {this.props.projectDialogState.status === 'saving' && <LinearProgress />}
            </Dialog>
            </form>
        );
    }
}

const validate = (formValues) => {
    const errors = {};

    if( !formValues.name){
        errors.name = "Project name is required."
        return errors;
    }

    if( formValues.name === 'Inbox'){
        errors.name = "Inbox is a reserved project name."
        return errors;
    }

    return errors;
}

ProjectFormDialog = reduxForm({
    form: FORM_PROJECT_DIALOG,
    validate
})(ProjectFormDialog)

ProjectFormDialog = connect(
    state => {
      return { 
          projects: state.projects, 
          projectDialogState: state.projectDialog,
          myForm: state.form.projectDialog,
          userProfile: state.userProfile
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(ProjectFormDialog)

  ProjectFormDialog.protoTypes = {
      title: PropTypes.string.isRequired,
      description: PropTypes.string.isRequired,
      onSubmit: PropTypes.func.isRequired, //<== onSubmit(formValues)
      componentDidUpdateCallback: PropTypes.func //<== componentDidUpdateCallback(prevProps, currentProps)
  }

export default (withStyles(useStyles))( withSnackbar(ProjectFormDialog));