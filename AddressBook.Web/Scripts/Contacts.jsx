var ContactBox = React.createClass({
    loadContactsFromServer: function () {
        $.ajax({
            url: this.props.url,
            dataType: 'json',
            cache: false,
            success: function (data) {
                this.setState({ data: JSON.parse(data) });
            }.bind(this),
            error: function (xhr, status, err) {
                console.error(this.props.url, status, err.toString());
            }.bind(this)
        });
    },

    getInitialState: function () {
        return { data: [] };
    },

    componentDidMount: function () {
        this.loadContactsFromServer();
        setInterval(this.loadContactsFromServer, this.props.pollInterval);
    },

    render: function () {
        return (
            <div className="contactBox">
                <h2>Contacts</h2>

                <ContactList data={this.state.data} />

                <ContactForm />
            </div>
        )
    }
});

var ContactList = React.createClass({
    render: function () {
        var contactNodes = this.props.data.map(function (contact) {
            return (
                <Contact key={contact.Id}
                         firstName={contact.FirstName}
                         lastName={contact.LastName}
                         nickname={contact.NickName}
                         email={contact.Email}
                         phoneNumbers={contact.PhoneNumbers} />
            );
        });
        var rowNumber = 0;
        return (
            <div className="contactList container">
                {contactNodes}
            </div>
        );
    }
});

var Contact = React.createClass({
    render: function () {
        var phoneNodes = this.props.phoneNumbers.map(function (phoneNumber) {
            return (
                <PhoneNumber id={phoneNumber.Id}
                             contactId={phoneNumber.ContactId}
                             phoneNumber={phoneNumber.Number} />
            );
        });
        return (
            <div className="row contact">
                <div className="col-md-2">{this.props.firstName} {this.props.lastName}</div>
                <div className="col-md-2">{this.props.nickname}</div>
                <div className="col-md-2">{this.props.email}</div>
                <div className="col-md-2">{phoneNodes}</div>
            </div>

        );
    }
});

var PhoneNumber = React.createClass({
    render: function () {
        var number = this.props.phoneNumber;
        var formattedNumber = number.substr(0, 3) + '.' + number.substr(3, 3) + '.' + number.substr(6, 4)
        return (
            <div><span>{formattedNumber}</span></div>
        );
    }
});


var ContactForm = React.createClass({
    render: function () {
        return (
            <form className="contactForm">
                <input type="text" placeholder="First Name" />
                <input type="text" placeholder="Last Name" />


                <input type="submit" value="Post" />
            </form>
        );
    }
});

ReactDOM.render(
    <ContactBox url="api/Contacts" pollInterval={2000} />,
    document.getElementById('content')
);