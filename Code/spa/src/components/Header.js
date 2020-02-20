import React from 'react'
import { AppBar, Toolbar, IconButton, Typography} from '@material-ui/core'
import MenuIcon from '@material-ui/icons/Menu';
import { makeStyles } from '@material-ui/core/styles';
import Security from './Security'

const useStyles = makeStyles(theme => ({
    root: {
      flexGrow: 1,
    },
    menuButton: {
      marginRight: theme.spacing(2),
    },
    title: {
      flexGrow: 1,
    },
  }));

const Header = ()=> {
    const classes = useStyles();
    
    return (
        <AppBar position="static">
            <Toolbar>
                <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="menu">
                    <MenuIcon  />
                </IconButton>
                <Typography variant="h6" className={classes.title}>
                    GTD To Do
                </Typography>
                <Security />
            </Toolbar>
        </AppBar>
    );
}

export default Header