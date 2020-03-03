import React from 'react'
import { AppBar, Toolbar, IconButton, Typography } from '@material-ui/core'
import MenuIcon from '@material-ui/icons/Menu';
import { withStyles } from '@material-ui/core/styles';
import LoginLogout from './LoginLogout';
import { connect } from 'react-redux';

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
          <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
              <MenuIcon />
          </IconButton>
          <Typography variant="h6" className={classes.title}>
            To Do List
          </Typography>
          <LoginLogout />
        </Toolbar>
      </AppBar>
    );
  }
}

const mapPropsToState = (state) => {
  const user = state.oidc.user;
  if (!user) {
    return { user: null, userName: null };
  }
  else {
    return { user: user, userName: user.profile.name };
  }
};

export default connect(mapPropsToState)(withStyles(useStyles)(Header));