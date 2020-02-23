import React from 'react'
import { AppBar, Toolbar, IconButton, Typography} from '@material-ui/core'
import MenuIcon from '@material-ui/icons/Menu';
import { withStyles } from '@material-ui/core/styles';
import LoginLogout from './LoginLogout';
import { connect } from 'react-redux';

const useStyles = theme => ({
    root: {
      flexGrow: 1,
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
  });

class Header extends React.Component{
  render(){
    
    const { classes } = this.props;
    const userNameToPrint = this.props.user ? " :: " + this.props.userName : "";
    return (
        <AppBar position="static">
            <Toolbar>
                <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
                    <MenuIcon  />
                </IconButton>
                <Typography variant="h6" className={classes.title}>
                    GTD To Do {userNameToPrint}
                </Typography>
                <LoginLogout />
            </Toolbar>
        </AppBar>
    );
  }
}

const mapPropsToState = (state) => {
  const user = state.oidc.user;
    if( !user ){
      return { user: null, userName: null };
    }
    else{
      return { user: user, userName: user.profile.name };
    }
};

export default connect(mapPropsToState)(withStyles(useStyles)(Header));