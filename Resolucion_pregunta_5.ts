// 1. Definimos los tipos de roles que puede tener un usuario.
type UserRoles = "admin" | "user" | "moderator";

// 2. Define la interfaz del usuario.
interface User {
  id: number;
  name: string;
  role: UserRoles;
  isActive: boolean;
}

type UsersResponse = User[];

class UserService {
  private users: UsersResponse = [];
  private lastId: number = 0;

  constructor() {
    this.users = [
      { id: 1, name: "Andrea", role: "admin", isActive: true },
      { id: 2, name: "Carlos", role: "user", isActive: false },
      { id: 3, name: "Beatriz", role: "moderator", isActive: true },
      { id: 4, name: "Daniela", role: "user", isActive: true }
    ];
    this.lastId = this.users.length;
  }

  getUser(id: number): any {
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].id === id) {
        return this.users[i];
      }
    }
    return null;
  }

  getUsersByRole(role: string): any[] {
    let result = [];
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].role == role) {
        result.push(this.users[i]);
      }
    }
    return result;
  }

  deleteUser(id: number): void {
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].id === id) {
        this.users.splice(i, 1);
      }
    }
  }

  addUser(user: any): void {
    if (user.id && user.name && user.role) {
      this.users.push(user);
    }
  }

  activateUser(id: number): void {
    let user = this.getUser(id);
    if (user) {
      user.isActive = true;
    }
  }

  deactivateUser(id: number): void {
    let user = this.getUser(id);
    if (user) {
      user.isActive = false;
    }
  }

  getActiveUsers(): any[] {
    let activeUsers = [];
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].isActive) {
        activeUsers.push(this.users[i]);
      }
    }
    return activeUsers;
  }
}

// Instrucciones:
// - Refactorizar el código para mejorar su eficiencia, legibilidad y mantenibilidad.
// - Aplicar tipado adecuado, métodos más claros y evitar repetición de código.
// - Explicar brevemente qué mejoras aplicaste y por qué.
