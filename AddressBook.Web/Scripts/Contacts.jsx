var ContactBox = React.createClass({
    render: function () {
        return (
            <div className="contactBox">
                <h2>Contacts</h2>


                <ContactList />

                <ContactForm />
            </div>
        )
    }
});

var ContactList = React.createClass({
    render: function () {
        return (
            <div className="contactList container">

            <Contact email="tdtolton@gmail.com">Torrey Tolton</Contact>
            <Contact email="adtolton@gmail.com">Andrew Tolton</Contact>
      </div>
    );
    }
});

var Contact = React.createClass({
    render: function () {
        return (
            <div className="row contact">
                <div className="col-md-2">{this.props.children}</div>
                <div className="col-md-1">{this.props.email}</div>
            </div>

        );
    }
});


var ContactForm = React.createClass({
    render: function () {
        return (
      <div className="contactForm">
          Hello, world! I am a contact tForm.
      </div>
    );
    }
});

ReactDOM.render(
    <ContactBox />,
    document.getElementById('content')
);