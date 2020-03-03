import React from 'react'
import Header from './Header'
import CssBaseline from '@material-ui/core/CssBaseline';
import { ThemeProvider, makeStyles } from '@material-ui/core/styles';
import theme from '../theme';
import { Provider } from 'react-redux';
import { OidcProvider } from 'redux-oidc';
import store from '../store';
import userManager from '../userManager';
import { Router, Route, Switch } from 'react-router-dom'
import CallbackPage from './Callback';
import history from '../history';
import HomePage from './Home';
import ToDo from './ToDo';
import PrivateRoute from './PrivateRoute';

const drawerWidth = 240;

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
  }
}));

const App = () => {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <ThemeProvider theme={theme}>
                <CssBaseline />
                <Provider store={store}>
                    <OidcProvider store={store} userManager={userManager}>
                        <Router history={history}>
                            <Header />
                            <Switch>
                                <Route path="/" exact component={HomePage} />
                                <Route path="/callback" component={CallbackPage} />
                                <PrivateRoute path="/todo" component={ToDo} />
                            </Switch>
                        </Router>
                    </OidcProvider>
                </Provider>
            </ThemeProvider>
        </div>
    );
};

export default App