import React from 'react'
import Header from './Header'
import Container from '@material-ui/core/Container';
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

const App = () => {
    return (
        <React.Fragment>
            <Provider store={store}>
                <OidcProvider store={store} userManager={userManager}>
                    <Container maxWidth="lg" style={{ height: '100vh' }}>
                        <Router history={history}>
                            <Header />
                            <Switch>
                                <Route path="/" exact component={HomePage} />
                                <Route path="/callback" component={CallbackPage} />
                                <PrivateRoute path="/todo" component={ToDo} />
                            </Switch>
                        </Router>
                    </Container>
                </OidcProvider>
            </Provider>
        </React.Fragment>
    );
};

export default App