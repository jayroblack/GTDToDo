import React from 'react'
import Header from './Header'
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import { Provider } from 'react-redux';
import { OidcProvider } from 'redux-oidc';
import store from '../store';
import userManager from '../userManager';
import { Router, Route } from 'react-router-dom'
import CallbackPage from './Callback';
import history from '../history';
import HomePage from './Home';

// https://www.gistia.com/react-authentication-security-private-routes/ for learning how to secure routes.

const App = ()=> {
    return( 
        <React.Fragment>
            <Provider store={store}>
            <OidcProvider store={store} userManager={userManager}>
            <Container maxWidth="lg">
                <Typography component="div" style={{ backgroundColor: '#d3d3d3', height: '100vh' }}>
                    <Header />
                    <Router history={history}>
                        <div>
                            <Route path="/" exact component={HomePage} />
                            <Route path="/#/callback" component={CallbackPage} />
                        </div>
                    </Router>
                </Typography>
            </Container>
            </OidcProvider>
            </Provider>
        </React.Fragment>
    );
};

export default App