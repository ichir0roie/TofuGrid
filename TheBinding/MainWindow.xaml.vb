Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Class MainWindow

    Public Property m As New Model
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        DataContext = m

        Dim b As New Binding
        'b.Source='明示的にソースを指定しない場合は、大体親要素のDataContextが入る。
        b.Path = New PropertyPath("im.textForCb")
        lbForCb.SetBinding(Label.ContentProperty, b)

        b = New Binding
        b.Source = m.im
        b.Path = New PropertyPath("textForCb")
        lbForCb2.SetBinding(Label.ContentProperty, b)

    End Sub

    'https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged?view=net-6.0

    Class Model
        Implements INotifyPropertyChanged


        'モデル側の変更をフロントに通知するには、INotifyPropertyChangedを用いた通知が必要。
        Private text_ As String
        Public Property text As String
            Get
                Return text_
            End Get
            Set(value As String)
                text_ = value

                'NotifyPropertyChanged("text")
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("text")) '肝心なのは、RaiseEventすること。
            End Set
        End Property


        Public Property dict As New Dictionary(Of String, String) From {
            {"a", "a"},
            {"b", "b"}
        }

        Public Property im As New InnerModel

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        '参考サイトに乗ってる関数
        Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub


    End Class

    Class InnerModel
        Public Property text As String = "this is default value."
        Public Property textForCb As String = "this is binded from code behind."

    End Class

    Private Sub reset_Click(sender As Object, e As RoutedEventArgs)
        m.text = "reset!"
    End Sub

End Class
