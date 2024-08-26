import React from 'react';
import { createRoot } from 'react-dom/client';
import FroalaEditor from 'react-froala-wysiwyg';
import 'froala-editor/css/froala_editor.pkgd.min.css';
import 'froala-editor/css/froala_style.min.css';
import 'froala-editor/js/plugins.pkgd.min.js';

class App extends React.Component {
    constructor() {
        super();
        this.state = {
            content: typeof window.initialContent !== 'undefined' ? window.initialContent : ''
        };

        this.editorRef = React.createRef(); // Froala Editor 인스턴스에 대한 참조를 생성합니다.
        this.handleImageUpload = this.handleImageUpload.bind(this);
    }

    handleModelChange = (model) => {
        this.setState({ content: model }, () => {
            document.getElementById('content').value = this.state.content;
        });
    };

    handleImageUpload = function (files) {
        const editor = this.editorRef.current?.editor; // FroalaEditor 인스턴스를 참조합니다.
        const file = files[0];
        const formData = new FormData();
        formData.append('file', file);

        fetch('/api/images', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
                return response.json(); // 응답을 JSON 형식으로 파싱
            })
            .then(data => {
                if (editor && editor.image) {
                    // 업로드된 이미지의 URL을 에디터에 삽입
                    const $img = editor.image.insert(data.link, null, null, editor.image.get(), null);
                } else {
                    console.error('Froala Editor instance is not available.');
                }
            })
            .catch(error => {
                console.error('Image upload failed:', error);
            });

        return false;
    };

    render() {
        return (
            <div>
                <h2>Froala Editor in React with Cloudinary</h2>
                <FroalaEditor
                    tag='textarea'
                    model={this.state.content}
                    onModelChange={this.handleModelChange}
                    config={{
                        pluginsEnabled: [
                            'align', 'charCounter', 'codeBeautifier', 'codeView', 'colors',
                            'draggable', 'embedly', 'emoticons', 'entities', 'file',
                            'fontFamily', 'fontSize', 'fullscreen', 'image', 'imageManager',
                            'inlineStyle', 'lineBreaker', 'link', 'lists', 'paragraphFormat',
                            'paragraphStyle', 'quickInsert', 'quote', 'save', 'table',
                            'url', 'video', 'wordPaste', 'undo'
                        ],
                        toolbarButtons: [
                            'undo', 'redo', '|', 'bold', 'italic', 'underline', '|',
                            'fontFamily', 'fontSize', 'color', '|',
                            'align', 'formatOL', 'formatUL', '|', 'outdent', 'indent',
                            'paragraphFormat', 'paragraphStyle', '|',
                            'insertLink', 'insertImage', 'insertVideo', '|',
                            'insertTable', 'quote', 'emoticons', '|',
                            'html'
                        ],
                        imageAllowedTypes: ['jpeg', 'jpg', 'png', 'gif'],
                        events: {
                            'image.beforeUpload': this.handleImageUpload,
                            'image.inserted': function ($img) {
                                $img.css('width', '80%'); // 이미지 너비를 80%로 설정
                            }
                        }
                    }}
                    ref={this.editorRef} // 에디터 인스턴스 참조 설정
                />
                <textarea
                    id="content"
                    name="Content"
                    style={{ display: 'none' }}
                    value={this.state.content}
                    readOnly
                />
            </div>
        );
    }
}

const container = document.getElementById('root');
const root = createRoot(container);
root.render(<App />);
