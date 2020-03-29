import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Field, reduxForm } from 'redux-form';
import { withStyles } from '@material-ui/core/styles';
import { Dialog, DialogTitle, DialogContent, 
    DialogContentText, DialogActions, Button, TextField, LinearProgress } from '@material-ui/core'
import { CloseNameFormDialog } from '../actions/nameFormDialog';
import { withSnackbar } from 'notistack';
import { NAME_FORM_DIALOG } from '../forms'

const useStyles = theme => ({

});


class NameFormDialog extends React.Component {

    constructor(props){
        super(props);
        this.timer = null;
    }

    handleDismiss = (cancelled) => {
        this.props.dispatch(CloseNameFormDialog(cancelled));
    };

    componentWillUnmount(){
        clearTimeout(this.timer);
    }

    componentDidUpdate(prevProps){
        if( this.props.componentDidUpdateCallback ){
            this.props.componentDidUpdateCallback(prevProps, this.props);
        }
    }

    renderTextField = ({ input, label, meta, ...custom }) => {
        const opts = {};
        if( meta.error && meta.touched){
            opts["error"] = true;
            opts["helperText"] = meta.error;
        }
        
        if( this.props.nameFormDialog.status === 'saveFailed' ){
            opts["error"] = true;
            opts["helperText"] = this.props.nameFormDialog.errorMessage;
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

        if( this.props.nameFormDialog.status === 'saving' ){
            disabledOptions = { disabled:true };
        }
        else{
            disabledOptions = { };
        }
        
        return (
            <form name={NAME_FORM_DIALOG} >
            <Dialog open={ this.props.isOpen } onClose={() => this.handleDismiss(false)} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">{this.props.title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {this.props.description}
                    </DialogContentText>
                    <Field name="name" label={ `${this.props.entity} Name` } component={this.renderTextField} type="text" {...disabledOptions} />
                </DialogContent>
                <DialogActions>
                    <Button variant="contained" onClick={ ()=> this.handleDismiss(true)} color="secondary" {...disabledOptions}>
                        Cancel
                    </Button>
                    <Button variant="contained" onClick={this.props.handleSubmit(this.props.onSubmit)} color="primary" {...disabledOptions}>
                        Save
                    </Button>
                </DialogActions>
                {this.props.nameFormDialog.status === 'saving' && <LinearProgress />}
            </Dialog>
            </form>
        );
    }
}

const validate = (formValues) => {
    const errors = {};

    if( !formValues.name){
        errors.name = "Name is required."
        return errors;
    }

    if( formValues.name === 'Inbox'){
        errors.name = "Inbox is a reserved name."
        return errors;
    }

    return errors;
}

NameFormDialog = reduxForm({
    form: NAME_FORM_DIALOG,
    validate
})(NameFormDialog)

NameFormDialog = connect(
    state => {
      return { 
          nameFormDialog: state.nameFormDialog
        };
    }, 
    dispatch => {
        return { dispatch }
    }
  )(NameFormDialog)

  NameFormDialog.protoTypes = {
      title: PropTypes.string.isRequired,
      entity: PropTypes.string.isRequired,
      description: PropTypes.string.isRequired,
      isOpen: PropTypes.func.isRequired,
      onSubmit: PropTypes.func.isRequired, //<== onSubmit(formValues)
      componentDidUpdateCallback: PropTypes.func //<== componentDidUpdateCallback(prevProps, currentProps)
  }

export default (withStyles(useStyles))( withSnackbar(NameFormDialog));