import React from 'react'
import Header from './Header'
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import { Provider } from 'react-redux';
import { OidcProvider } from 'redux-oidc';
import store from '../store';
import userManager from '../userManager';
import { Router, Route, Switch } from 'react-router-dom'
import CallbackPage from './Callback';
import history from '../history';
import HomePage from './Home';
import Root from './root';
import ToDo  from './ToDo';
import PrivateRoute from './PrivateRoute';

// https://www.gistia.com/react-authentication-security-private-routes/ for learning how to secure routes.
// Need to learn how to use Hooks first.  
const App = ()=> {
    return( 
        <React.Fragment>
            <Provider store={store}>
            <OidcProvider store={store} userManager={userManager}>
            <Root>
            <Container maxWidth="lg">
                <Typography component="div" style={{ backgroundColor: '#d3d3d3', height: '100vh' }}>
                    <Router history={history}>
                        <Header />
                        <Switch>
                            <Route path="/" exact component={HomePage} />
                            <Route path="/callback" component={CallbackPage} />
                            <PrivateRoute path="/todo" component={ToDo} />
                        </Switch>
                    </Router>
                </Typography>
            </Container>
            </Root>
            </OidcProvider>
            </Provider>
        </React.Fragment>
    );
};

export default App