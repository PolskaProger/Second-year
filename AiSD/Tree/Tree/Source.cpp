#include <iostream>
#include <algorithm>

template<typename T>
class AVLTree {
private:
    struct Node {
        T data;
        Node* left;
        Node* right;
        int height;
        Node(const T& value) : data(value), left(nullptr), right(nullptr), height(1) {}
    };

    Node* root;

    int height(Node* n) {
        return n ? n->height : 0; 
    }

    int getBalance(Node* n) {
        return n ? height(n->left) - height(n->right) : 0;
    }

    Node* rightRotate(Node* y) {
        Node* x = y->left;
        Node* T2 = x->right;

        x->right = y;
        y->left = T2;

        y->height = std::max(height(y->left), height(y->right)) + 1;
        x->height = std::max(height(x->left), height(x->right)) + 1;

        return x;
    }

    Node* leftRotate(Node* x) {
        Node* y = x->right;
        Node* T2 = y->left;

        y->left = x;
        x->right = T2;

        x->height = std::max(height(x->left), height(x->right)) + 1;
        y->height = std::max(height(y->left), height(y->right)) + 1;

        return y;
    }

    Node* insert(Node* node, const T& key) {
        if (!node) return new Node(key);

        if (key < node->data) {
            node->left = insert(node->left, key);
        }
        else if (key > node->data) {
            node->right = insert(node->right, key);
        }
        else {
            return node;
        }

        node->height = 1 + std::max(height(node->left), height(node->right));

        int balance = getBalance(node);

        if (balance > 1 && key < node->left->data) {
            return rightRotate(node);
        }

        if (balance < -1 && key > node->right->data) {
            return leftRotate(node);
        }

        if (balance > 1 && key > node->left->data) {
            node->left = leftRotate(node->left);
            return rightRotate(node);
        }

        if (balance < -1 && key < node->right->data) {
            node->right = rightRotate(node->right);
            return leftRotate(node);
        }

        return node;
    }

    Node* minValueNode(Node* node) {
        Node* current = node;
        while (current->left != nullptr)
            current = current->left;
        return current;
    }

    Node* deleteNode(Node* root, const T& key) {
        if (!root) return root;

        if (key < root->data) {
            root->left = deleteNode(root->left, key);
        }
        else if (key > root->data) {
            root->right = deleteNode(root->right, key);
        }
        else {
            if (!root->left || !root->right) {
                Node* temp = root->left ? root->left : root->right;

                if (!temp) {
                    temp = root;
                    root = nullptr;
                }
                else {
                    *root = *temp;
                }
                delete temp;
            }
            else {
                Node* temp = minValueNode(root->right);
                root->data = temp->data;
                root->right = deleteNode(root->right, temp->data);
            }
        }

        if (!root) return root;

        root->height = 1 + std::max(height(root->left), height(root->right));

        int balance = getBalance(root);

        if (balance > 1 && getBalance(root->left) >= 0)
            return rightRotate(root);

        if (balance > 1 && getBalance(root->left) < 0) {
            root->left = leftRotate(root->left);
            return rightRotate(root);
        }

        if (balance < -1 && getBalance(root->right) <= 0)
            return leftRotate(root);

        if (balance < -1 && getBalance(root->right) > 0) {
            root->right = rightRotate(root->right);
            return leftRotate(root);
        }

        return root;
    }

    void inOrder(Node* root) const {
        if (root) {
            inOrder(root->left);
            std::cout << root->data << " ";
            inOrder(root->right);
        }
    }

public:
    AVLTree() : root(nullptr) {}

    void insert(const T& key) {
        root = insert(root, key);
    }

    void deleteNode(const T& key) {
        root = deleteNode(root, key);
    }

    void inOrder() const {
        inOrder(root);
        std::cout << std::endl;
    }
};

int main() {
    AVLTree<int> tree;
    tree.insert(1);
    tree.insert(2);
    tree.insert(3);
    tree.insert(5);
    tree.insert(6);
    tree.insert(7);
    tree.insert(8);

    std::cout << "Preorder traversal of the constructed AVL tree is \n";
    tree.inOrder();

    tree.deleteNode(3);

    std::cout << "Preorder traversal after deletion of node \n";
    tree.inOrder();

    return 0;
}
