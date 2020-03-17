import React from 'react'
import { AppBar, Toolbar, IconButton, Typography } from '@material-ui/core'
import { withStyles } from '@material-ui/core/styles';
import LoginLogout from './LoginLogout';
import { connect } from 'react-redux';
import SettingsIcon from '@material-ui/icons/Settings';
import AddIcon from '@material-ui/icons/Add';



const useStyles = theme => ({
  root: {
    flexGrow: 1,
  },
  appBar: {
    zIndex: theme.zIndex.drawer + 1,
  },
  menuButton: {
    marginRight: theme.spacing(2),
  },
  title: {
    flexGrow: 1,
  },
});

class Header extends React.Component {

  render() {
    const { classes } = this.props;
    return (
      <AppBar position="fixed" className={classes.appBar} >
        <Toolbar>
          <Typography variant="h6" className={classes.title}>
            To Do List
          </Typography>
          <IconButton disabled={this.props.buttonDisabled}
            aria-label="Add Task" 
            color="inherit" 
          >
            <AddIcon />
          </IconButton>
          <IconButton aria-label="settings" color="inherit" disabled={this.props.buttonDisabled} >
            <SettingsIcon />
          </IconButton>
          <LoginLogout />
        </Toolbar>
      </AppBar>
    );
  }
}

const mapPropsToState = (state) => {
  const user = state.oidc.user;
  
  if (!user) {
    return { buttonDisabled: true, newProjectDialog: state.NewProjectDialog };
  }
  else {
    return { buttonDisabled: false, newProjectDialog: state.NewProjectDialog };
  }
};

function mapDispatchToProps(dispatch) {
  return {
    dispatch
  };
}

export default connect(mapPropsToState, mapDispatchToProps)(withStyles(useStyles)(Header));